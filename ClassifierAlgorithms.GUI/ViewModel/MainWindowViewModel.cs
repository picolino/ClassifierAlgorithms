#region Usings

using System;
using System.ComponentModel;
using DevExpress.Mvvm;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

#endregion

namespace ClassifierAlgorithms.GUI.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        private const string ScatterSerieTag = "ScatterSeries";
        private const string LineSerieTag = "LineSeries";

        public MainWindowViewModel()
        {
            PlotModel = InitializePlot(0, 1);
        }

        public PlotModel PlotModel { get; }

        private PlotModel InitializePlot(int min, int max)
        {
            var plotModel = new PlotModel();
            plotModel.Axes.Add(new LinearAxis {Position = AxisPosition.Left, Maximum = max, Minimum = min});
            plotModel.Axes.Add(new LinearAxis {Position = AxisPosition.Bottom, Maximum = max, Minimum = min});

            plotModel.Series.Add(new ScatterSeries
                                 {
                                     Tag = ScatterSerieTag,
                                     MarkerType = MarkerType.Cross,
                                     MarkerSize = 2,
                                     MarkerFill = OxyColors.Transparent,
                                     MarkerStrokeThickness = 1,
                                     MarkerStroke = OxyColors.DarkBlue
                                 });
            plotModel.Series.Add(new LineSeries
                                 {
                                     Tag = LineSerieTag,
                                     MarkerType = MarkerType.None
                                 });

            return plotModel;
        }

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get { throw new NotImplementedException(); }
        }

        public string Error { get; }

        #endregion
    }
}