#region Usings

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using ClassifierAlgorithms.Core.Extensions;
using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

#endregion

namespace ClassifierAlgorithms.GUI.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        private const string ScatterSeriesTag = "ScatterSeries";
        private const string LineSeriesTag = "LineSeries";

        public AsyncCommand GeneratePointsCommand { get; }

        public MainWindowViewModel()
        {
            PlotModel = InitializePlot(0, 1);
            
            FirstClassExpectation = 0.7;
            FirstClassDispersion = 0.05;

            SecondClassExpectation = 0.3;
            SecondClassDispersion = 0.05;

            GeneratePointsCommand = new AsyncCommand(OnGeneratePoints);
        }

        private async Task OnGeneratePoints()
        {
            await Task.Run(() =>
                            {
                               const int countOfPoints = 500;
                               var random = new Random();

                               ScatterSeries.Points.Clear();
                               for (var i = 0; i < countOfPoints; i++)
                               {
                                   var newPoint = new ScatterPoint(random.NextGaussian(FirstClassExpectation, FirstClassDispersion),
                                                                   random.NextGaussian(FirstClassExpectation, FirstClassDispersion));
                                   ScatterSeries.Points.Add(newPoint);
                               }

                               for (var i = 0; i < countOfPoints; i++)
                               {
                                   var newPoint = new ScatterPoint(random.NextGaussian(SecondClassExpectation, SecondClassDispersion),
                                                                   random.NextGaussian(SecondClassExpectation, SecondClassDispersion));
                                   ScatterSeries.Points.Add(newPoint);
                               }
                           });
            PlotModel.InvalidatePlot(true);
        }

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
    }
}