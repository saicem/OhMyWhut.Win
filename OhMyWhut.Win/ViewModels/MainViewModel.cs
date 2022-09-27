namespace OhMyWhut.Win.ViewModels
{
    public class MainViewModel
    {
        public BookViewModel BookViewModel { get; } = new BookViewModel();

        public CourseViewModel CourseViewModel { get; } = new CourseViewModel();

        public ElectricFeeViewModel ElectricFeeViewModel { get; } = new ElectricFeeViewModel();
    }
}
