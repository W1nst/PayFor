﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayFor.Models;

namespace PayFor.Context
{

    public class PayForRepository : IPayForRepository
    {
        private readonly PayForContext _context;

        public PayForRepository(PayForContext context)
        {
            _context = context;
        }

        //Groups
        public async Task<Group> GetGroup(int id, string userId)
        {
            if (!await IsInGroup(id, userId)) return null;

            var group = await _context.Groups
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.Category)
                    .Include(x => x.UserGroups)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == id);
            group.Payments = group.Payments.OrderByDescending(x=>x.Date).ToList();
            return group;
        }

        public async Task<IEnumerable<Group>> GetUserGroups(string userId)
        {
            var user = await _context.Users
                .Include(x => x.UserGroups)
                .ThenInclude(x => x.Group)
                .ThenInclude(x=>x.AuthorUser)
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user.UserGroups.Select(x => x.Group).OrderByDescending(x=>x.CreateDateTime).Distinct().ToList();
        }

        public async Task CreateGroup(Group group, string userId)
        {
            var user = await _context.Users.Include(x => x.UserGroups).FirstOrDefaultAsync(x => x.Id == userId);
            var userGroup = new UserGroup() { User = user, Group = group };

            user.UserGroups.Add(userGroup);
            group.UserGroups = new List<UserGroup>() { userGroup };
            group.AuthorUser = user;

            _context.Groups.Add(group);
            _context.Users.Update(user);
        }

        public async Task<bool> DeleteGroup(int id, string userId)
        {
            //if (!await IsGroupOwner(id, userId)) return false;
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id && x.AuthorUserId == userId);
            if (group == null) return false;
            _context.Groups.Remove(group);
            return true;
        }

        //Payments
        public async Task<Payment> GetPayment(int id, string userId)
        {
            var payment = await _context.Payments
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Group)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (payment == null)
                return null;
            return !await IsInGroup(payment.Group.Id, userId) ? null : payment;
        }

        public async Task<IEnumerable<Payment>> GetUserPayments(string userId)
        {
            return await _context.Payments
                .Include(x => x.Category)
                .Include(x => x.User)
                .Include(x => x.Group)
                .Where(x => x.User.Id == userId).OrderByDescending(x=>x.Date).ToListAsync();
        }

        public async Task<bool> CreatePayment(Payment payment, string userId)
        {
            if (!await IsInGroup(payment.GroupId, userId)) return false;
            payment.UserId = userId;
            _context.Payments.Add(payment);
            return true;
        }

        public async Task<bool> DeletePayment(int id, string userId)
        {
            var payment = await _context.Payments.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id == id);
            if (payment == null)
                return false;
            if (!await IsInGroup(payment.Group.Id, userId))
                return false;
            _context.Payments.Remove(payment);
            return true;
        }
        public async Task<bool> EditPayment(Payment payment, string userId){
            var p = await GetPayment(payment.Id,userId);
            if (p == null) return false;
                p.Amount = payment.Amount;
                p.Date = payment.Date;
                p.Note = payment.Note;
                p.CategoryId = payment.CategoryId;
            return true;
        }
        //Categories
        public async Task<Category> GetCategory (int id, string userId){
            return await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id);
        }
        public async Task<IEnumerable<Category>> GetAllCategories(string userId)
        {
            return await _context.Categories.ToListAsync();
        }
        public bool CreateCategory(Category category, string userId){
            _context.Categories.Add(category);
            return true;
        }
        public async Task<bool> DeleteCategory(int id, string userId){
            var category = await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            return true;
        }

        //User
        public async Task<User> GetUser(string userId){
            return await _context.Users
                .Include(x => x.UserGroups)
                .ThenInclude(x => x.Group)
                .Include(x=>x.Payments)
                .ThenInclude(x=>x.Category)
                .FirstOrDefaultAsync(x=>x.Id == userId);
        }

        //Other
        async Task<bool> IsInGroup(int groupId, string userId)
        {
            var exist = await _context.UserGroup
                .AnyAsync(x=>x.UserId == userId && x.GroupId == groupId);
            return exist;
        }

        async Task<bool> IsGroupOwner(int groupId, string userId)
        {
            var exist = await _context.Groups
                .AnyAsync(x => x.Id == groupId && x.AuthorUserId == userId);
            return exist;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }

}
