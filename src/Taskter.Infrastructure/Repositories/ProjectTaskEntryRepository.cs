﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taskter.Core.Entities;
using Taskter.Core.Interfaces;
using Taskter.Infrastructure.Data;

namespace Taskter.Infrastructure.Repositories
{
    class ProjectTaskEntryRepository: IProjectTaskEntryRepository
    {
        private readonly TaskterDbContext _context;

        public ProjectTaskEntryRepository(TaskterDbContext context)
        {
            _context = context; 
        }


        public IEnumerable<ProjectTaskEntry> GetProjectTaskEntriesByDate(int userId, DateTime date)
        {
            return _context.ProjectTaskEntres.Where((pr => pr.Id == userId)).Where(p=>p.Date==date);

        }

    }
}
