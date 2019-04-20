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
        public double FirstClassExpectationX
        {
            get { return GetProperty<double>(() => FirstClassExpectationX); }
            set { SetProperty(() => FirstClassExpectationX, value); }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Должно быть указано математическое ожидание первого параметра")]
        public double FirstClassExpectationY
        {
            get { return GetProperty<double>(() => FirstClassExpectationY); }
            set { SetProperty(() => FirstClassExpectationY, value); }
        }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Должно быть указано математическое ожидание второго параметра")]
        public double SecondClassExpectationX
        {
            get { return GetProperty<double>(() => SecondClassExpectationX); }
            set { SetProperty(() => SecondClassExpectationX, value); }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Должно быть указано математическое ожидание второго параметра")]
        public double SecondClassExpectationY
        {
            get { return GetProperty<double>(() => SecondClassExpectationY); }
            set { SetProperty(() => SecondClassExpectationY, value); }
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Должна быть указана зависимость первого параметра")]
        public double FirstClassDependency
        {
            get { return GetProperty(() => FirstClassDependency); }
            set { SetProperty(() => FirstClassDependency, value); }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Должна быть указана зависимость второго параметра")]
        public double SecondClassDependency
        {
            get { return GetProperty(() => SecondClassDependency); }
            set { SetProperty(() => SecondClassDependency, value); }
        }
    }
}