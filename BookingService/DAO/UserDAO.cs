using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using BookingService.Models;


namespace BookingService.DAO
{
    public interface IUserDAO
    {
        IEnumerable<LoginByUsernamePassword_Result> LoginByUsernamePassword(string username, string password);
    }

    public class UserDAO : IUserDAO
    {
        protected readonly ISession session;
        protected readonly IMapper mapper;

        public UserDAO()
        {
            ICassandraDAO cassandraDAO = new CassandraDAO();
            session = cassandraDAO.GetSession();
            mapper = new Mapper(session);
        }

        public virtual IEnumerable<LoginByUsernamePassword_Result> LoginByUsernamePassword(string username, string password)
        {
            return mapper.Fetch<LoginByUsernamePassword_Result>(
                "SELECT * FROM user.login WHERE username = ? AND password = ? ALLOW FILTERING", username, password
            );
        }
    }
}