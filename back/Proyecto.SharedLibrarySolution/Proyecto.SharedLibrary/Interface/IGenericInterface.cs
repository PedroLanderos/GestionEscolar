using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Llaveremos.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace Llaveremos.SharedLibrary.Interface
{
    //Llamada publica de las operaciones mas comunes (crud) de cada servicio a traves de una interfaz para mantener el principio SOLID
    public interface IGenericInterface<T> where T : class
    {
        //Task<response> es una operacion asincronica que va a devolver un response cuando termine y T entity es la entidad que se va a crear 
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(); //ienuramerable<t> es una coleccion de objetos de tipo T 
        Task<T> FindByIdAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate); //un predicado sirve para hacer busquedas especificas de forma dinamica, toma la entidad T y devuelve un valor bool con la respuesta del request

    }
}
