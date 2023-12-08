using BarbieQ.Repositories;

namespace BarbieQ.Models.ViewModels
{
    public class LayoutViewModel
    {
        public CategoriaRepository _catRepos { get; }
        public LayoutViewModel(CategoriaRepository c)
        {
            _catRepos = c;
        }
        public int Contador { get; set; } = 0;
        private int totalCategorias;
        
        public int TotalCategorias
        {
            get { return _catRepos.GetAll().Count(); }
            set { totalCategorias = value; }
        }
        private IEnumerable<string> cat;

        public IEnumerable<string> Categorias
        {
            get { return _catRepos.GetAll().Select(x=> x.Nombre); }
            set { cat = value; }
        }

        private int mitad;

        public int Mitad
        {
            get { return TotalCategorias/2; }
            set { mitad = value; }
        }

    }
}
