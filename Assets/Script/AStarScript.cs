using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AStarScript
{
	// Validator for path nodes
	// Needed to cope with nodes that might be GameObjects and therefore
	// not 'acutally' null when compared in generic methods
	public static bool Invalid(PathNode inNode)
	{
		if(inNode == null || inNode.Invalid)
			return true;
		return false;
	}
 
	// Distance between Nodes
	static float Distance(PathNode start, PathNode goal)
	{
		if(Invalid(start) || Invalid(goal))
			return float.MaxValue;
		return Vector3.Distance(start.Position, goal.Position);
	}
 
	// Base cost Estimate - this would need to be evoled for your project based on true cost
	// to move between nodes
	static float HeuristicCostEstimate(PathNode start, PathNode goal)
	{
		return Distance(start, goal);
	}
 
	// Find the current lowest score path
	static PathNode LowestScore(List<PathNode> openset, Dictionary<PathNode, float> scores)
	{
		int index = 0;
		float lowScore = float.MaxValue;
 
		for(int i = 0; i < openset.Count; i++)
		{
			if(scores[openset[i]] > lowScore)
				continue;
			index = i;
			lowScore = scores[openset[i]];
		}
 
		return openset[index];
	}
 
 
	// Calculate the A* path
	public static List<PathNode> Calculate(PathNode start, PathNode goal)
	{
		List<PathNode> closedset = new List<PathNode>();    // The set of nodes already evaluated.
		List<PathNode> openset = new List<PathNode>();    // The set of tentative nodes to be evaluated.
		openset.Add(start);
     	Dictionary<PathNode, PathNode> came_from = new Dictionary<PathNode, PathNode>();    // The map of navigated nodes.
 
		Dictionary<PathNode, float> g_score = new Dictionary<PathNode, float>();
		g_score[start] = 0.0f; // Cost from start along best known path.
 
		Dictionary<PathNode, float> h_score = new Dictionary<PathNode, float>();
		h_score[start] = HeuristicCostEstimate(start, goal); 
 
		Dictionary<PathNode, float> f_score = new Dictionary<PathNode, float>();
		f_score[start] = h_score[start]; // Estimated total cost from start to goal through y.
 
		while(openset.Count != 0)
		{
			PathNode x = LowestScore(openset, f_score);
			if(x.Equals(goal))
			{
				List<PathNode> result = new List<PathNode>();
				ReconstructPath(came_from, x, ref result);
				return result;
			}
			openset.Remove(x);
			closedset.Add(x);
			foreach(PathNode y in x.Connections)
			{
				if(AStarScript.Invalid(y) || closedset.Contains(y))
					continue;
				float tentative_g_score = g_score[x] + Distance(x, y);
 
				bool tentative_is_better = false;
				if(!openset.Contains(y))
				{
					openset.Add(y);
					tentative_is_better = true;
				}
				else if (tentative_g_score < g_score[y])
					tentative_is_better = true;
 
				if(tentative_is_better)
				{
					came_from[y] = x;
					g_score[y] = tentative_g_score;
					h_score[y] = HeuristicCostEstimate(y, goal);
					f_score[y] = g_score[y] + h_score[y];
				}
			}
		}
 
     return null;
 
	}
 
	// Once the goal has been found we now reconstruct the steps taken to get to the path
	static void ReconstructPath(Dictionary<PathNode, PathNode> came_from, PathNode current_node, ref List<PathNode> result)
	{
		if(came_from.ContainsKey(current_node))
		{
			ReconstructPath(came_from, came_from[current_node], ref result);
			result.Add(current_node);
			return;
		}
		result.Add(current_node);
	}
}