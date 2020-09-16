using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMusik.WebAPI.Helpers
{
    public class Paginador<T> where T: class
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public IEnumerable<T> Registros { get; set; }
        public Paginador(IEnumerable<T> registros, int totalRegistros, int paginaActual, int registrosPorPagina)
        {
            this.Registros = registros;
            this.TotalRegistros = totalRegistros;
            this.PaginaActual = paginaActual;
            this.RegistrosPorPagina = registrosPorPagina;
        }

        public int TotalPaginas
        {
            get
            {
                return (int)Math.Ceiling(this.TotalRegistros / (double)this.RegistrosPorPagina);
            }
        }

        public bool TienePaginaAnterior
        {
            get
            {
                return (this.PaginaActual > 1);
            }
        }

        public bool TienePaginaSiguiente
        {
            get
            {
                return (this.PaginaActual < this.TotalPaginas);
            }
        }
    }
}
