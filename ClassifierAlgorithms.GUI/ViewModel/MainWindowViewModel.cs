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
            
            FirstClassExpectation = 0.7;
            FirstClassDispersion = 0.05;

            SecondClassExpectation = 0.3;
            SecondClassDispersion = 0.05;

            GeneratePointsCommand = new AsyncCommand(OnGeneratePoints);
            ClassifyCommand = new AsyncCommand(OnClassify, CanClassify);
        }

        private Class FirstClass { get; set; }
        private Class SecondClass { get; set; }

        private bool CanClassify()
        {
            return false;
        }

        private async Task OnClassify()
        {
            throw new NotImplementedException();
        }

        private async Task OnGeneratePoints()
        {
            await Task.Run(() =>
                           {
                               const int countOfPoints = 1000;

                               if (ScatterSeries.Points.Count != 0)
                               {
                                   ScatterSeries.Points.Clear();
                               }

                               var generator = new Generator();

                               var firstClassGenerateTask = Task.Run(() =>
                                                                     {
                                                                         FirstClass = generator.GenerateClassByGaussian(countOfPoints, FirstClassExpectation, FirstClassDispersion);
                                                                         for (var i = 0; i < countOfPoints; i++)
                                                                         {
                                                                             var newPoint = new ScatterPoint(FirstClass.Vector[i, 0],
                                                                                                             FirstClass.Vector[i, 1]);
                                                                             ScatterSeries.Points.Add(newPoint);
                                                                         }
                                                                     });

                               var secondClassGenerateTask = Task.Run(() =>
                                                                     {
                                                                         SecondClass = generator.GenerateClassByGaussian(countOfPoints, SecondClassExpectation, SecondClassDispersion);
                                                                         for (var i = 0; i < countOfPoints; i++)
                                                                         {
                                                                             var newPoint = new ScatterPoint(SecondClass.Vector[i, 0],
                                                                                                             SecondClass.Vector[i, 1]);
                                                                             ScatterSeries.Points.Add(newPoint);
                                                                         }
                                                                     });

                               Task.WaitAll(firstClassGenerateTask, secondClassGenerateTask);
                               PlotModel.InvalidatePlot(true);
                           });
        }

        #region Plot
        
        private const string ScatterSeriesTag = "ScatterSeries";
        private const string LineSeriesTag = "LineSeries";
        private ScatterSeries ScatterSeries { get; set; }
        private LineSeries LineSeries { get; set; }

        private PlotModel InitializePlot(int min, int max)
        {
            var plotModel = new PlotModel();

            plotModel.Axes.Add(new LinearAxis {Position = AxisPosition.Left, Maximum = max, Minimum = min});
            plotModel.Axes.Add(new LinearAxis {Position = AxisPosition.Bottom, Maximum = max, Minimum = min});

            ScatterSeries = new ScatterSeries
                            {
                                Tag = ScatterSeriesTag,
                                MarkerType = MarkerType.Cross,
                                MarkerSize = 2,
                                MarkerFill = OxyColors.Transparent,
                                MarkerStrokeThickness = 1,
                                MarkerStroke = OxyColors.DarkBlue
                            };
            LineSeries = new LineSeries
                         {
                             Tag = LineSeriesTag,
                             MarkerType = MarkerType.None
                         };

            plotModel.Series.Add(ScatterSeries);
            plotModel.Series.Add(LineSeries);

            return plotModel;
        }

        #endregion
    }
}