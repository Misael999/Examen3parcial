using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PM2E2201810110123.Models;
using SQLite;

namespace PM2E2201810110123.Data
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<CPagos>().Wait();
        }

        public Task<int> SavePersona(CPagos person)
        {
            if (person.Idpago != 0)
            {
                return db.UpdateAsync(person);
                ;
            }
            else
            {
                return db.InsertAsync(person);
            }
        }
        /// <summary>
        /// Recuperar todos los personas
        /// </summary>
        /// <returns></returns>
        public Task<List<CPagos>> GetPersonasAync()
        {
            return db.Table<CPagos>().ToListAsync();
        }
        /// <summary>
        /// Recupera las personas por la identidad
        /// </summary>
        /// <param name="identidad">Identidad de la persona requerida</param>
        /// <returns></returns>
        public Task<CPagos> GetPersonaByIdAsync(int person)
        {
            return db.Table<CPagos>().Where(a => a.Idpago == person).FirstOrDefaultAsync();
        }

        public Task<int> Grabarpersona(CPagos person)
        {
            if (person.Idpago !=0)
            {
                return db.UpdateAsync(person);
            }
            else
            {
                return db.InsertAsync(person);
            }    
        }

        public Task<int> DropPersonaAsync(CPagos person)
        {
            return db.DeleteAsync(person);
        }


    }
}
