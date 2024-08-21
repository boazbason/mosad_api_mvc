using ProjectMosadApi.Models;

namespace ProjectMosadApi;

public static class DBTemporary_list
{
    public static List<M_Target> Targets { get; set; } = new List<M_Target>();
    public static List<M_Agent> Agents { get; set; } = new List<M_Agent>();
    public static List<M_Mission> Missions { get; set; } = new List<M_Mission>();
    //Agents
    public static void AddAgent(M_Agent agent)
    {
        Agents.Add(agent);
    }

    public static List<M_Agent> GetAgents()
    {
        return Agents;
    }
        
        
    public static void AddTarget(M_Target target)
    {
        Targets.Add(target);
    }

    public static void AddMission(M_Mission mission)
    {
        Missions.Add(mission);
    }
}