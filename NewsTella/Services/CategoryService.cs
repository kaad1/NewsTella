﻿using Microsoft.EntityFrameworkCore;
using NewsTella.Data;
using NewsTella.Models.Database;
using NewsTella.Models.ViewModel;

namespace NewsTella.Services
{
	public class CategoryService: ICategoryService
	{
		private readonly AppDbContext _db;

		public CategoryService(AppDbContext db)
		{
			_db = db;
		}
		public List<Category> GetCategories()
		{
			var obj = _db.Categories.ToList();
			return obj;
		}
		public Category GetCategoryById(int id)
		{
			var category = _db.Categories.FirstOrDefault(c => c.Id == id);
			return category;
		}

		public void AddCategory(Category category)
		{
			_db.Categories.Add(category);
			_db.SaveChanges();
		}

		public void RemoveCategory(Category category)
		{
			_db.Categories.Remove(category);
			_db.SaveChanges();
		}

		public void UpdateCategory(Category category)
		{
			_db.Categories.Update(category);
			_db.SaveChanges();
		}
	}
}