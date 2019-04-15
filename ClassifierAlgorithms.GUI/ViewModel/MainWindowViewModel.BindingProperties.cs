using System.ComponentModel.DataAnnotations;
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Должно быть указано математическое ожидание первого параметра")]
        public double FirstClassExpectation
        {
            get { return GetProperty<double>(() => FirstClassExpectation); }
            set { SetProperty(() => FirstClassExpectation, value); }
        }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Должно быть указано математическое ожидание второго параметра")]
        public double SecondClassExpectation
        {
            get { return GetProperty<double>(() => SecondClassExpectation); }
            set { SetProperty(() => SecondClassExpectation, value); }
        }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Должна быть указана дисперсия первого параметра")]
        public double FirstClassDispersion
        {
            get { return GetProperty(() => FirstClassDispersion); }
            set { SetProperty(() => FirstClassDispersion, value); }
        }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Должна быть указана дисперсия второго параметра")]
        public double SecondClassDispersion
        {
            get { return GetProperty(() => SecondClassDispersion); }
            set { SetProperty(() => SecondClassDispersion, value); }
        }
    }
}