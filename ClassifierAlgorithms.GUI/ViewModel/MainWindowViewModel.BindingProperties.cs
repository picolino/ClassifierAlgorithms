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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Должна быть указана зависимость параметров")]
        public string CorrelationMatrixInput
        {
            get { return GetProperty(() => CorrelationMatrixInput); }
            set { SetProperty(() => CorrelationMatrixInput, value); }
        }

        public double[,] CorrelationMatrix
        {
            get
            {
                var correlationMatrixInput = CorrelationMatrixInput.Split('\n', ' ');
                var correlationMatrix = new double[2, 2];
                correlationMatrix[0, 0] = double.Parse(correlationMatrixInput[0]);
                correlationMatrix[0, 1] = double.Parse(correlationMatrixInput[1]);
                correlationMatrix[1, 0] = double.Parse(correlationMatrixInput[2]);
                correlationMatrix[1, 1] = double.Parse(correlationMatrixInput[3]);
                return correlationMatrix;
            }
        }
    }
}