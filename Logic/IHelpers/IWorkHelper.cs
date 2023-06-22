using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IWorkHelper
    {
        bool CreateWorks(WorkersViewModel workersViewModel, string user);
        bool EditWork(WorkDetails workData);
        bool DeleteWork(WorkDetails workId);
        bool ApplyForJob(JobApplication jobApplication, string user);
        bool PostJobApplicant(JobApplication jobApplicant);
        JobApplication GetJobApplicant(int applicantId);
        bool DeleteJob(JobApplication jobId);
    }
}
