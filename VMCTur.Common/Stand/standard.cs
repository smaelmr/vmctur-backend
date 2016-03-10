using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Common.Standard
{
    public static class Standard
    {
        /// <summary>
        /// Smael: obtém a descrição de um determinado Enumerador.
        /// </summary>
        /// <param name="valor">Smael: enumerador que terá a descrição obtida.</param>
        /// <returns>Smael: string com a descrição do Enumerador.</returns>
        public static string ObterDescricaoEnum(Enum pValor)
        {
            System.Reflection.FieldInfo fieldInfo = pValor.GetType().GetField(pValor.ToString());

            DescriptionAttribute[] atributos = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return atributos.Length > 0 ? atributos[0].Description ?? "Nulo" : pValor.ToString();
        }
    }
}
