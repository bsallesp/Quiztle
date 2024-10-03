using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness;

namespace Quiztle.DataContext.DataService.Repository
{
    public class UserRepository
    {
        private readonly PostgreQuiztleContext _context;

        public UserRepository(PostgreQuiztleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Método para criar um novo usuário
        public async Task CreateUserAsync(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));

                _context.Users!.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateUserAsync: An exception occurred while creating the user:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // Método para obter um usuário por Id
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await _context.Users!
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUserByIdAsync: An exception occurred while retrieving the user by Id:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // Método para obter todos os usuários
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users!
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllUsersAsync: An exception occurred while retrieving all users:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // Método para atualizar um usuário existente
        public async Task UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users!
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                if (existingUser == null)
                {
                    throw new InvalidOperationException("UpdateUserAsync: The User to update does not exist.");
                }

                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateUserAsync: An exception occurred while updating the user:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // Método para deletar um usuário
        public async Task DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _context.Users!.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    throw new InvalidOperationException("DeleteUserAsync: The User to delete does not exist.");
                }

                _context.Users!.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeleteUserAsync: An exception occurred while deleting the user:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // Salva as alterações no contexto do banco de dados
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
