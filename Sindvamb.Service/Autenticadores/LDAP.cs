#region

using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;

#endregion

namespace Sindvamb.Service.Autenticadores
{
    class LDAP
    {
        public static dynamic Autenticar(string urlLDAP, dynamic usuarioDTO)
        {
            using DirectoryEntry directoryEntry = new DirectoryEntry(urlLDAP, usuarioDTO.DS_LOGIN_USUARIO, usuarioDTO.DS_SENHA_USUARIO);
            DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry)
            { Filter = "(SAMAccountName=" + usuarioDTO.DS_LOGIN_USUARIO + ")" };

            try
            {
                SearchResult searchResult = directorySearcher.FindOne();

                usuarioDTO.CD_MATRICULA = Guid.Parse(usuarioDTO.DS_LOGIN_USUARIO);
                usuarioDTO.NOME = string.Format("AD: {0}", usuarioDTO.DS_LOGIN_USUARIO);

                return usuarioDTO;
            }
            catch (COMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}