using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public class WorkHelper:IWorkHelper
    {
        private readonly AppDbContext _context;

        public WorkHelper(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateWorks(WorkersViewModel workersViewModel, string user)
        {
            if (user != string.Empty)
            {
                if (workersViewModel != null)
                {
                    var workData = new WorkDetails()
                    {
                        IsActive = true,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                        Name = workersViewModel.Name,
                        Salary = workersViewModel.Salary,
                        Email = workersViewModel.Email,
                    };
                    _context.Add(workData);
                    _context.SaveChanges();
                    return true;
                }
            }
            
            return false;
        }

        public bool EditWork(WorkDetails workData)
        {
            if (workData != null)
            {
                var getWorkFromDatabase = _context.WorkDetails.Where(x => x.Id == workData.Id && !x.IsDeleted).FirstOrDefault();
                if (getWorkFromDatabase != null)
                {
                    getWorkFromDatabase.Salary = workData.Salary;
                    getWorkFromDatabase.Name = workData.Name;
                    _context.Update(getWorkFromDatabase);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteWork(WorkDetails workId)
        {
            if (workId != null)
            {
                var deleteWorkInDatabase = _context.WorkDetails.Where(x => x.Id == workId.Id && !x.IsDeleted).FirstOrDefault(); 
                if (deleteWorkInDatabase != null) 
                {
                    deleteWorkInDatabase.Salary = workId.Salary;
                    deleteWorkInDatabase.Name = workId.Name;
                    _context.Remove(deleteWorkInDatabase);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool ApplyForJob(JobApplication jobApplication, string user)
        {
            if (user != string.Empty)
            {
                if (jobApplication != null)
                {
                    var jobData = new JobApplication()
                    {
                        IsActive = true,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                        Name = jobApplication.Name,
                        PhoneNumber = jobApplication.PhoneNumber,
                        Email = user,
                        WorkDetailsId = jobApplication.WorkDetailsId,
                    };
                    _context.JobApplication.Add(jobData);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        
        public JobApplication GetJobApplicant(int applicantId)
        {
            if (applicantId > 0)
            {
                var getApplicant = _context.JobApplication.Where(c => c.Id == applicantId && !c.IsDeleted).FirstOrDefault();
                if(getApplicant != null)
                {
                    return getApplicant;
                }
            }
            return null;
        }

        public bool PostJobApplicant(JobApplication jobApplicant)
        {
            if (jobApplicant != null)
            {
                var getJobApplicant = _context.JobApplication.Where(x => x.Id == jobApplicant.Id && !x.IsDeleted).FirstOrDefault();
                if (getJobApplicant != null)
                {
                    getJobApplicant.Name = jobApplicant.Name;
                    getJobApplicant.PhoneNumber = jobApplicant.PhoneNumber;
                    getJobApplicant.WorkDetailsId = jobApplicant.WorkDetailsId;
                    _context.Update(getJobApplicant);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteJob(JobApplication jobId)
        {
            try
            {
                if (jobId != null)
                {
                    var removeJob = _context.JobApplication.Where(c => c.Id == jobId.Id && !c.IsDeleted).FirstOrDefault();
                    if (removeJob != null)

                    {
                        removeJob.IsDeleted = true;
                        removeJob.IsActive = false;
                        _context.JobApplication.Update(removeJob);
                        _context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }

}
