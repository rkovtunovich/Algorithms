namespace Scheduling.Common;

// Real world applications example:
// A small business-say, a photocopying service with a single large machine-faces the following scheduling problem. 
// Each morning they get a set of jobs from customers. They want to do the jobs on their single machine in an order that keeps their customers happiest. 
// Customer i's job will take ti time to complete. 
// Given a schedule (i.e., an ordering of the jobs), let ti denote the finishing time of job i
// For example, if job j is the first to be done, we would have Cj=tj and if job j is done right after job i we would have Cj=Ci+t. 
// Each customer i also has a given weight wi that represents his or her importance to the business. 
// The happiness of customer i is expected to be dependent on the finishing time of i's job. 
// So the company decides that they want to order the jobs to minimize the weighted sum of the completion times, SUM(wi*Ci)  
// Design an efficient algorithm to solve this problem. 
// That is, you are given a set of n jobs with a processing time ti and a weight wi for each job. 
// You want to order the jobs so as to minimize the weighted sum of the completion times, SUM(wi*Ci) 
// Example. Suppose there are two jobs: the first takes time t1=1 and has weight w1=10 while the second job takes time t2=3 and has weight w2=2
// Then doing job 1 first would yield a weighted completion time of 10*1+2*4=18 while doing the second job first would yield the larger weighted completion time of 10*4+2*3=46
//
//  Greedy algorithm with the well-known "weighted shortest processing time first" (WSPT) rule.
//  In this rule, jobs are ordered based on the ratio of their weight to their processing time (wi/ti), with higher ratios taking priority.
//  This strategy is known to minimize the weighted sum of completion times, SUM(wi*Ci) 
// Algorithm
// Calculate Ratio: For each job, calculate the ratio wi/ti
// Sort Jobs: Sort the jobs in descending order of this ratio. 
// Schedule Jobs: Schedule the jobs in this order. The completion time Ci for each job is the sum of the processing times of all the jobs scheduled before it,
// plus its own processing time.
// Compute Weighted Sum: Calculate the weighted sum of completion times, SUM(wi*Ci) for this schedule.
// 
// Why it Works
// Intuition: Jobs with a high weight should be completed earlier to minimize their impact on the weighted sum.
// However, if such a job also takes a long time, delaying many other jobs, it might not be optimal.
// The ratio wi/ti balances these factors, prioritizing jobs that have a high weight relative to their processing time.
// 
// Formal Proof: The optimality of this approach can be proven more formally using an exchange argument.
// Suppose there is an optimal schedule that does not follow the WSPT rule.
// We can find two consecutive jobs in this schedule, say job i and job j, where job i comes before job j, but wi/ti < wj/tj.
// By swapping these two jobs, we can show that the total weighted completion time decreases, contradicting the assumption that the original schedule was optimal.
// This implies that the schedule following the WSPT rule is indeed optimal.
// 
// Time Complexity
// Calculating the Ratio: O(n), where n is the number of jobs.
// Sorting Jobs O(nlogn), dominated by the sorting step.
// Scheduling Jobs and Computing Weighted Sum: O(n), as it involves a single pass through the sorted list.
// Therefore, the overall time complexity of the algorithm is O(nlogn), making it efficient for this problem.

public static class SingleMachineWeightedShortestProcessingTimeFirst
{
    public static List<Job> GetSchedule(List<Job> jobs)
    {
        var weightedJobs = new (double, Job)[jobs.Count];

        for (int i = 0; i < jobs.Count; i++)
        {
            weightedJobs[i] = (jobs[i].Duration / (double)jobs[i].Weight, jobs[i]);
        }

        Array.Sort(weightedJobs, (x, y) => x.Item1.CompareTo(y.Item1));

        return weightedJobs.Select(x => x.Item2).ToList();
    }
}
