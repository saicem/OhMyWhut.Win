namespace OhMyWhut.Win.ViewModels
{
    public class MainViewModel
    {
        public BookViewModel BookViewModel { get; }

        public CourseViewModel CourseViewModel { get; }

        public ElectricFeeViewModel ElectricFeeViewModel { get; }

        public MainViewModel()
        {
            BookViewModel = new BookViewModel();
            CourseViewModel = new CourseViewModel();
            ElectricFeeViewModel = new ElectricFeeViewModel();
        }
    }
}
