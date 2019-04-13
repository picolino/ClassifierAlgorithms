using OxyPlot;

namespace ClassifierAlgorithms.GUI.ViewModel
{
    public partial class MainWindowViewModel
    {
        public PlotModel PlotModel
        {
            get { return GetProperty<PlotModel>(() => PlotModel); }
            set { SetProperty(() => PlotModel, value); }
        }

        public double FirstClassExpectation
        {
            get { return GetProperty<double>(() => FirstClassExpectation); }
            set { SetProperty(() => FirstClassExpectation, value); }
        }

        public double SecondClassExpectation
        {
            get { return GetProperty<double>(() => SecondClassExpectation); }
            set { SetProperty(() => SecondClassExpectation, value); }
        }

        public double FirstClassDispersion
        {
            get { return GetProperty(() => FirstClassDispersion); }
            set { SetProperty(() => FirstClassDispersion, value); }
        }

        public double SecondClassDispersion
        {
            get { return GetProperty(() => SecondClassDispersion); }
            set { SetProperty(() => SecondClassDispersion, value); }
        }
    }
}