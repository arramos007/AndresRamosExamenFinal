using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AndresRamosExamenFinal.Model
{
    public class ProductoModel
    {
        [PrimaryKey, AutoIncrement]
        public int cid { get; set; }
        public int id { get; set; }
        public string codigo_principal_producto { get; set; }
        public string codigo_auxiliar_producto { get; set; }
        public string nombre { get; set; }
        public float valor_unitario { get; set; }
        public int tipo_productos_id { get; set; }
        public int users_id { get; set; }
        public int isCompleted { get; set; }
        public DateTimeOffset created_at { get; set; }
        public DateTimeOffset updated_at { get; set; }
        public bool Delete { get; set; }
        public bool Update { get; set; }
        public bool Store { get; set; }
    }
}
