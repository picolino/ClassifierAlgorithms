#region Usings

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ClassifierAlgorithms.Core;
using ClassifierAlgorithms.Core.Domain;
using ClassifierAlgorithms.Core.Extensions;
using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

#endregion

namespace ClassifierAlgorithms.GUI.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public AsyncCommand GeneratePointsCommand { get; }
        public AsyncCommand ClassifyCommand { get; }

        public MainWindowViewModel()
        {
            PlotModel = InitializePlot(0, 1);
            
            FirstClassExpectation = 0.3;
            FirstClassDispersion = 0.05;

            SecondClassExpectation = 0.7;
            SecondClassDispersion = 0.05;

            FirstClassDependency = 0;
            SecondClassDependency = 0;

            GeneratePointsCommand = new AsyncCommand(OnGeneratePoints);
            ClassifyCommand = new AsyncCommand(OnClassify, CanClassify);
        }

        private Class FirstClass { get; set; }
        private Class SecondClass { get; set; }

        private bool CanClassify()
        {
            return true;
        }

        private async Task OnClassify()
        {
            var bayes = new BayesClassifier(FirstClass, SecondClass);
            var r1 = bayes.Classify(0.1, 0.1);
            var r2 = bayes.Classify(0.7, 0.7);

            bayes.ClassifyByCorrelation(0.5, 0.4);
        }

        private async Task OnGeneratePoints()
        {
            await Task.Run(() =>
                           {
                               const int countOfPoints = 1000;

                               if (FirstClassScatterSeries.Points.Count != 0)
                               {
                                   FirstClassScatterSeries.Points.Clear();
                               }

                               if (SecondClassScatterSeries.Points.Count != 0)
                               {
                                   SecondClassScatterSeries.Points.Clear();
                               }

                               var generator = new Generator();

                               var firstClassGenerateTask = Task.Run(() =>
                                                                     {
                                                                         FirstClass = generator.GenerateClassByGaussian(countOfPoints, FirstClassExpectation, FirstClassDispersion);
                                                                         FirstClass.Dependency = FirstClassDependency;
                                                                         for (var i = 0; i < countOfPoints; i++)
                                                                         {
                                                                             var newPoint = new ScatterPoint(FirstClass.Vector[i, 0],
                                                                                                             FirstClass.Vector[i, 1]);
                                                                             FirstClassScatterSeries.Points.Add(newPoint);
                                                                         }
                                                                     });

                               var secondClassGenerateTask = Task.Run(() =>
                                                                     {
                                                                         SecondClass = generator.GenerateClassByGaussian(countOfPoints, SecondClassExpectation, SecondClassDispersion);
                                                                         FirstClass.Dependency = SecondClassDependency;
                                                                         for (var i = 0; i < countOfPoints; i++)
                                                                         {
                                                                             var newPoint = new ScatterPoint(SecondClass.Vector[i, 0],
                                                                                                             SecondClass.Vector[i, 1]);
                                                                             SecondClassScatterSeries.Points.Add(newPoint);
                                                                         }
                                                                     });

                               Task.WaitAll(firstClassGenerateTask, secondClassGenerateTask);
                               PlotModel.InvalidatePlot(true);
                           });
        }

        #region Plot
        
        private const string FirstClassScatterSeriesTag = "FirstClassScatterSeries";
        private const string SecondClassScatterSeriesTag = "SecondClassScatterSeries";
        private const string LineSeriesTag = "LineSeries";
        private ScatterSeries FirstClassScatterSeries { get; set; }
        private ScatterSeries SecondClassScatterSeries { get; set; }
        private LineSeries LineSeries { get; set; }

        private PlotModel InitializePlot(int min, int max)
        {
            var plotModel = new PlotModel();

            plotModel.Axes.Add(new LinearAxis {Position = AxisPosition.Left, Maximum = max, Minimum = min});
            plotModel.Axes.Add(new LinearAxis {Position = AxisPosition.Bottom, Maximum = max, Minimum = min});

            FirstClassScatterSeries = new ScatterSeries
                            {
                                Tag = FirstClassScatterSeriesTag,
                                MarkerType = MarkerType.Cross,
                                MarkerSize = 2,
                                MarkerFill = OxyColors.Transparent,
                                MarkerStrokeThickness = 1,
                                MarkerStroke = OxyColors.DarkBlue
                            };
            SecondClassScatterSeries = new ScatterSeries
                                      {
                                          Tag = SecondClassScatterSeriesTag,
                                          MarkerType = MarkerType.Cross,
                                          MarkerSize = 2,
                                          MarkerFill = OxyColors.Transparent,
                                          MarkerStrokeThickness = 1,
                                          MarkerStroke = OxyColors.DarkOrange
                                      };
            LineSeries = new LineSeries
                         {
                             Tag = LineSeriesTag,
                             MarkerType = MarkerType.None
                         };

            plotModel.Series.Add(FirstClassScatterSeries);
            plotModel.Series.Add(SecondClassScatterSeries);
            plotModel.Series.Add(LineSeries);

            return plotModel;
        }

        #endregion
    }
}