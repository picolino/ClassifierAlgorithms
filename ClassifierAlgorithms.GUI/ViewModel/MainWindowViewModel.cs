#region Usings

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ClassifierAlgorithms.Core;
using ClassifierAlgorithms.Core.Domain;
using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using WannEx.Core.HighPerformance.Evolve.Genetic;

#endregion

namespace ClassifierAlgorithms.GUI.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Random random;

        public ICommand GeneratePointsCommand { get; }
        public ICommand ClassifyBayesCommand { get; }
        public ICommand ClassifyLogisticRegressionCommand { get; }
        public ICommand ClassifyWannCommand { get; }

        public MainWindowViewModel()
        {
            PlotModel = InitializePlot(0, 1);
            random = new Random();

            FirstClassExpectationX = 0.3;
            FirstClassExpectationY = 0.3;

            SecondClassExpectationX = 0.7;
            SecondClassExpectationY = 0.7;

            CorrelationMatrixInput = "0,005 0\n0 0,005";

            GeneratePointsCommand = new DelegateCommand(OnGeneratePoints);
            ClassifyBayesCommand = new AsyncCommand(OnClassifyBayes, CanClassify);
            ClassifyLogisticRegressionCommand = new AsyncCommand(OnClassifyLogisticRegression, CanClassify);
            ClassifyWannCommand = new AsyncCommand(OnClassifyWann, CanClassify);
        }

        private Class FirstClass { get; set; }
        private Class SecondClass { get; set; }

        private bool CanClassify()
        {
            return FirstClass != null && SecondClass != null;
        }

        private async Task OnClassifyBayes()
        {
            await Task.Run(() =>
                           {
                               try
                               {
                                   var bayes = new BayesClassifier(FirstClass, SecondClass);
                                   var stopwatch = new Stopwatch();

                                   stopwatch.Start();
                                   for (var i = 0; i < 5000; i++)
                                   {
                                       var randomPointX = random.NextDouble() * (PlotModel.Axes[0].Maximum - PlotModel.Axes[0].Minimum) + PlotModel.Axes[0].Minimum;
                                       var randomPointY = random.NextDouble() * (PlotModel.Axes[1].Maximum - PlotModel.Axes[1].Minimum) + PlotModel.Axes[1].Minimum;

                                       var result = bayes.ClassifyByCorrelation(randomPointX, randomPointY, new Matrix(CorrelationMatrix));

                                       if (result == FirstClass)
                                       {
                                           FirstClassScatterSeries.Points.Add(new ScatterPoint(randomPointX, randomPointY, 4, double.NaN, FirstClass.Id));
                                       }
                                       else
                                       {
                                           SecondClassScatterSeries.Points.Add(new ScatterPoint(randomPointX, randomPointY, 4, double.NaN, FirstClass.Id));
                                       }

                                       if (stopwatch.Elapsed > TimeSpan.FromMilliseconds(100))
                                       {
                                           PlotModel.InvalidatePlot(true);
                                           stopwatch.Restart();
                                       }
                                   }

                                   PlotModel.InvalidatePlot(true);
                               }
                               catch (Exception e)
                               {
                                   MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                               }
                           });
        }

        private async Task OnClassifyLogisticRegression()
        {
            try
            {
                var logisticRegression = new LogisticRegression(FirstClass, SecondClass);
                logisticRegression.Train(0.01);
                var stopwatch = new Stopwatch();

                await Task.Run(() =>
                               {
                                   stopwatch.Start();
                                   for (var i = 0; i < 5000; i++)
                                   {
                                       var randomPointX = random.NextDouble() * (PlotModel.Axes[0].Maximum - PlotModel.Axes[0].Minimum) + PlotModel.Axes[0].Minimum;
                                       var randomPointY = random.NextDouble() * (PlotModel.Axes[1].Maximum - PlotModel.Axes[1].Minimum) + PlotModel.Axes[1].Minimum;

                                       var logisticRegressionProbabilityResult = logisticRegression.Classify(randomPointX, randomPointY);

                                       if (logisticRegressionProbabilityResult > 0.5)
                                       {
                                           FirstClassScatterSeries.Points.Add(new ScatterPoint(randomPointX, randomPointY, 4, double.NaN, FirstClass.Id));
                                       }
                                       else
                                       {
                                           SecondClassScatterSeries.Points.Add(new ScatterPoint(randomPointX, randomPointY, 4, double.NaN, FirstClass.Id));
                                       }


                                       if (stopwatch.Elapsed > TimeSpan.FromMilliseconds(100))
                                       {
                                           PlotModel.InvalidatePlot(true);
                                           stopwatch.Restart();
                                       }
                                   }
                               });

                PlotModel.InvalidatePlot(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task OnClassifyWann()
        {
            try
            {
                var trainer = new NeatTrainer();
                var unitedArray = new double[FirstClass.Vector.GetLength(0) + SecondClass.Vector.GetLength(0), 3];

                var counter = 0;

                for (var i = 0; i < FirstClass.Vector.GetLength(0); i++)
                {
                    unitedArray[counter, 0] = FirstClass.Vector[i, 0];
                    unitedArray[counter, 1] = FirstClass.Vector[i, 1];
                    unitedArray[counter, 2] = 0;
                    counter++;
                }

                for (var i = 0; i < SecondClass.Vector.GetLength(0); i++)
                {
                    unitedArray[counter, 0] = SecondClass.Vector[i, 0];
                    unitedArray[counter, 1] = SecondClass.Vector[i, 1];
                    unitedArray[counter, 2] = 1;
                    counter++;
                }

                var index = 0;
                while (index < unitedArray.GetLength(0))
                {
                    var selectedIndexFrom = random.Next(unitedArray.GetLength(0));
                    var x1 = unitedArray[index, 0];
                    var x2 = unitedArray[index, 1];
                    var cls = unitedArray[index, 2];
                    unitedArray[index, 0] = unitedArray[selectedIndexFrom, 0];
                    unitedArray[index, 1] = unitedArray[selectedIndexFrom, 1];
                    unitedArray[index, 2] = unitedArray[selectedIndexFrom, 2];
                    unitedArray[selectedIndexFrom, 0] = x1;
                    unitedArray[selectedIndexFrom, 1] = x2;
                    unitedArray[selectedIndexFrom, 2] = cls;
                    index++;
                }

                var trainInput = new double[unitedArray.GetLength(0) / 2][];
                var trainExpected = new double[unitedArray.GetLength(0) / 2][];

                for (var i = 0; i < trainInput.GetLength(0); i++)
                {
                    trainInput[i] = new[] { unitedArray[i, 0], unitedArray[i, 1] };
                    trainExpected[i] = new[] { unitedArray[i, 2] };
                }

                var weights = new[] { -2.0, -1.0, -0.5, 0.5, 1.0, 2.0 };
                trainer.Train(trainInput, trainExpected, weights, 0.1);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnGeneratePoints()
        {
            try
            {
                const int countOfPoints = 500;

                if (FirstClassScatterSeries.Points.Count != 0)
                {
                    FirstClassScatterSeries.Points.Clear();
                }

                if (SecondClassScatterSeries.Points.Count != 0)
                {
                    SecondClassScatterSeries.Points.Clear();
                }

                var generator = new Generator();

                Parallel.Invoke(() =>
                                {
                                    FirstClass = generator.GenerateClassByGaussian(countOfPoints, FirstClassExpectationX, FirstClassExpectationY, CorrelationMatrix);
                                    for (var i = 0; i < countOfPoints; i++)
                                    {
                                        var newPoint = new ScatterPoint(FirstClass.Vector[i, 0],
                                                                        FirstClass.Vector[i, 1]);
                                        FirstClassScatterSeries.Points.Add(newPoint);
                                    }
                                },
                                () =>
                                {
                                    SecondClass = generator.GenerateClassByGaussian(countOfPoints, SecondClassExpectationX, SecondClassExpectationY, CorrelationMatrix);
                                    for (var i = 0; i < countOfPoints; i++)
                                    {
                                        var newPoint = new ScatterPoint(SecondClass.Vector[i, 0],
                                                                        SecondClass.Vector[i, 1]);
                                        SecondClassScatterSeries.Points.Add(newPoint);
                                    }
                                });

                PlotModel.InvalidatePlot(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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