using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace VMCTur.Infra.Conn
{
    public class MySqlConn
    {
        #region Atributos

        private MySqlDataReader _dataReader;

        private static MySqlConn _instancia;

        private static MySqlConnection _connection;

        #endregion

        #region Propriedades

        public MySqlConnection Conexao
        {
            get { return _connection; }
        }

        public static Boolean TemConexao
        {
            get { return _instancia != null; }
        }

        #endregion

        #region Construtores

        public MySqlConn()
        {
        }

        #endregion

        #region Métodos

        public void BeginWork()
        {
            ExecutaQuery("BEGIN WORK;");
        }

        public void CommitWork()
        {

            MySqlCommand command = new MySqlCommand("COMMIT;", _connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Exception e = ex.GetBaseException();

                //Smael: Se erro foi causado por problemas de conexão
                if (e.GetType().IsSubclassOf(typeof(System.Net.Sockets.SocketException)) && ((System.Net.Sockets.SocketException)e).ErrorCode == 10054)
                {
                    //Smael: Fecha conexão atual
                    _connection.Close();
                    _connection = null;
                    //Smael: Conecta novamente
                    Conectar();
                }

                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void RollBack()
        {
            MySqlCommand command = new MySqlCommand("ROLLBACK;", _connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Exception e = ex.GetBaseException();

                //Smael: Se erro foi causado por problemas de conexão
                if (e.GetType().IsSubclassOf(typeof(System.Net.Sockets.SocketException)) && ((System.Net.Sockets.SocketException)e).ErrorCode == 10054)
                {
                    //Smael: Fecha conexão atual
                    _connection.Close();
                    _connection = null;
                    //Smael: Conecta novamente
                    Conectar();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecutaQuery(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, _connection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Exception e = ex.GetBaseException();

                this.RollBack();
                throw ex;
            }
            catch (Exception ex)
            {
                this.RollBack();
                throw ex;
            }
        }

        public MySqlDataReader ExecutaQueryComLeitura(string sql, CommandBehavior pCommand)
        {
            MySqlCommand command = new MySqlCommand(sql, _connection);

            try
            {
                _dataReader = command.ExecuteReader(pCommand);
            }
            catch (MySqlException ex)
            {
                Exception e = ex.GetBaseException();
            }
            catch
            {
                this.RollBack();
                throw new Exception("Erro ao fazer leitura");
            }

            return _dataReader;
        }

        public MySqlDataReader ExecutaQueryComLeitura(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, _connection);

            try
            {
                _dataReader = command.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                this.RollBack();
                throw new Exception(ex.Message + "\nErro ao fazer leitura");
            }

            return _dataReader;
        }

        public object ExecutaQueryScalar(string pSql)
        {
            MySqlCommand command = new MySqlCommand(pSql, _connection);

            object retorno = null;

            try
            {
                retorno = command.ExecuteScalar();
            }
            catch (MySqlException ex)
            {
                Exception e = ex.GetBaseException();

                this.RollBack();
                throw new Exception(ex.Message + "\nErro ao fazer leitura");
            }

            return retorno;
        }

        public void ExecutaQuery(MySqlCommand pCommand)
        {
            if (pCommand.Connection == null)
                pCommand.Connection = _connection;

            try
            {
                pCommand.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                this.RollBack();
                throw ex;
            }
        }

        public MySqlDataReader ExecutaQueryComLeitura(MySqlCommand pCommand, CommandBehavior pCommandBehavior)
        {
            if (pCommand.Connection == null)
                pCommand.Connection = _connection;

            try
            {
                _dataReader = pCommand.ExecuteReader(pCommandBehavior);
            }
            catch (MySqlException ex)
            {
                this.RollBack();
                throw ex;
            }

            return _dataReader;
        }

        public MySqlDataReader ExecutaQueryComLeitura(MySqlCommand pCommand)
        {
            if (pCommand.Connection == null)
                pCommand.Connection = _connection;

            try
            {
                _dataReader = pCommand.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                this.RollBack();
                throw new Exception(ex.Message + "\n\nErro ao fazer leitura");
            }
            catch (Exception ex)
            {
                this.RollBack();
                throw new Exception(ex.Message + "\n\nErro ao fazer leitura");
            }

            return _dataReader;
        }

        public object ExecutaQueryScalar(MySqlCommand pCommand)
        {
            if (pCommand.Connection == null)
                pCommand.Connection = _connection;

            object retorno = null;

            try
            {
                retorno = pCommand.ExecuteScalar();
            }
            catch (MySqlException ex)
            {
                this.RollBack();
                throw ex;
            }

            return retorno;
        }

        #endregion

        #region Métodos Estáticos

        /// <summary>
        /// Método para obter a Instancia com usuário e senha.
        /// </summary>
        /// <returns></returns>
        public static MySqlConn GetInstancia()
        {
            if (_instancia == null || _instancia.Conexao == null || _instancia.Conexao.State != ConnectionState.Open)
            {
                _instancia = new MySqlConn();
                Conectar();
            }

            return _instancia;
        }

        public static void Desconectar()
        {
            if (_connection != null)
                _connection.Close();
        }

        private static void Conectar()
        {
            _connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["AppConnectionString"].ConnectionString);

            try
            {
                _connection.Open();

                //Smael: Método para o evento de troca de estado da conexão
                _connection.StateChange += new StateChangeEventHandler(_connection_StateChange);
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        static void _connection_StateChange(object sender, StateChangeEventArgs e)
        {
            if (_connection.State != ConnectionState.Open)
                throw new Exception("Erro na conexão2 com o banco de dados");
        }

        #endregion
    }
}
