using blogPessoal.Data;
using blogPessoal.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogPessoal.Repository
{
    public class PostagemRepository : IPostagemRepository
    {
        public readonly Data.AppContext _context;
        private readonly ITemaRepository temaRepository;

        public PostagemRepository(AppContext context, ITemaRepository temaRepository)
        {
            _context = context;
            this.temaRepository = temaRepository;
        
        }

        public Postagem Create(Postagem postagem)
        {
            Postagem aux = _context.Postagens.FirstOrDefaultAsync(c => c.Id.Equals(postagem.Id)).Result;
            if (aux != null)
                return aux;

            if (postagem.Tema != null)
                postagem.Tema = this.temaRepository.Create(postagem.Tema);



            _context.Postagens.Add(postagem);
            _context.SaveChangesAsync();

            return postagem;
        }


        public void Delete(int id)
        {
            var postagemDelete = _context.Postagens.FindAsync(id);
            _context.Postagens.Remove(postagemDelete.Result);
            _context.SaveChangesAsync();
        }

        public List<Postagem> GetAll()
        {
            return _context.Postagens.Include(p => p.Tema).ToListAsync().Result;
        }

        public Postagem Get(int id)
        {
            try
            {
                var PostagemReturn = _context.Postagens.Include(p => p.Tema).FirstAsync(i => i.Id == id);
                return PostagemReturn.Result;
            }
            catch
            {
                return null;
            }

        }

        public List<Postagem> GetTitulo(string Titulo)
        {
            var PostagemReturn = _context.Postagens.Include(p => p.Tema).Where(p => p.Titulo.ToLower().Contains(Titulo.ToLower())).ToListAsync();
            return PostagemReturn.Result;
        }

        public Postagem Update(Postagem postagem)
        {
            if (postagem.Tema != null)
                postagem.Tema = this.temaRepository.Create(postagem.Tema);

            _context.Entry(postagem).State = EntityState.Modified;
            _context.SaveChangesAsync();

            return postagem;

        }
    }
}