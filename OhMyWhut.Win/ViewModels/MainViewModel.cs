namespace OhMyWhut.Win.ViewModels
{
    public class MainViewModel
    {
        public BookViewModel BookViewModel { get; set; } = new BookViewModel();

        public CourseViewModel CourseViewModel { get; set; } = new CourseViewModel();

        public ElectricFeeViewModel ElectricFeeViewModel { get; set; } = new ElectricFeeViewModel();
    }
}
