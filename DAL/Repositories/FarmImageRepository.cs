﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
	public class FarmImageRepository : GenericRepository<FarmImage>, IFarmImageRepository
	{
		public FarmImageRepository(KoiDbContext context) : base(context)
		{
		}
	}
}