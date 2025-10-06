using System.Diagnostics;

namespace AdditionalTask
{
	public static class Program
	{
		private const string fileName = "input.txt";

		public static void Main()
		{
			// Ожидает наличие файла input.txt с форматом данных:

			//# Вершины: A, B, C, D, E
			//# Ребра: Начало Конец Стоимость
			//A B 5
			//A C 10
			//B C 3
			//B D 2
			//C D 1
			//C E 7
			//D E 4

			if(ReadData(fileName, out var vertices, out var edges))
			{
				var graph = BuildGraph(vertices, edges);

				// Вычисляем кратчайшие расстояния между всеми парами вершин
				var allPairsDistances = new Dictionary<string, Dictionary<string, int>>();
				foreach(var vertex in vertices)
					allPairsDistances[vertex] = Dijkstra(graph, vertex);
					
				// Начальный маршрут
				var initialRoute = vertices.ToList();
				initialRoute.Add(vertices[0]);
				var initialCost = CalculateRouteCost(initialRoute, allPairsDistances);

				Console.WriteLine("Начальный маршрут: " + string.Join(" -> ", initialRoute));
				Console.WriteLine("Общая стоимость начального маршрута: " + initialCost);

				var stopwatch = Stopwatch.StartNew();

				//var optimizedRoute = TwoOpt(initialRoute, allPairsDistances);// Оптимизация маршрута 2-Opt
				var optimizedRoute = TwoPointerOptimization(initialRoute, allPairsDistances); // 2-Opt с двумя указателями

				stopwatch.Stop();
				var optimizedCost = CalculateRouteCost(optimizedRoute, allPairsDistances);

				Console.WriteLine("\nОптимизированный маршрут: " + string.Join(" -> ", optimizedRoute));
				Console.WriteLine("Общая стоимость оптимизированного маршрута: " + optimizedCost);
				Console.WriteLine("Время выполнения: " + stopwatch.Elapsed.TotalSeconds + " сек");
			}
		}

		private static Dictionary<string, List<NeighborConnection>> BuildGraph(string[] vertices, List<Edge> edges)
		{
			var graph = new Dictionary<string, List<NeighborConnection>>();
			foreach(var vertex in vertices)
			{
				var list = new List<NeighborConnection>();
				foreach(var edge in edges)
				{
					if(edge.From == vertex)
						list.Add(new NeighborConnection(edge.To, edge.Cost));
					else if(edge.To == vertex)
						list.Add(new NeighborConnection(edge.From, edge.Cost));
				}
				graph[vertex] = list;
			}
			return graph;
		}

		public static bool ReadData(string path, out string[] vertices, out List<Edge> edges)
		{
			vertices = null!;
			edges = null!;

			try
			{
				if(!File.Exists(path))
					throw new Exception($"File \"{path}\" not found");

				var lines = File.ReadAllLines(path)
					.Where(l => !string.IsNullOrWhiteSpace(l))
					.ToArray();

				vertices = lines[0].Replace("# Вершины: ", "")
					.Split(',')
					.Select(s => s.Trim())
					.ToArray();

				edges = new List<Edge>();
				foreach(var line in lines.Skip(2))
				{
					var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					if(parts.Length != 3)
						throw new Exception("Invalid format");

					var from = parts[0].Trim();
					var to = parts[1].Trim();

					if(!vertices.Contains(from) || !vertices.Contains(to))
					{
						throw new Exception("Invalid format");
					}

					edges.Add(new Edge
					{
						From = from,
						To = to,
						Cost = int.Parse(parts[2])
					});
				}
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public static Dictionary<string, int> Dijkstra(Dictionary<string, List<NeighborConnection>> graph, string start)
		{
			var distances = graph.Keys.ToDictionary(v => v, v => int.MaxValue);
			distances[start] = 0;

			var pq = new PriorityQueue<string, int>();
			pq.Enqueue(start, 0);

			while(pq.Count > 0)
			{
				pq.TryDequeue(out var u, out var distU);
				foreach(var neighbor in graph[u])
				{
					int newDist = distU + neighbor.Weight;
					if(newDist < distances[neighbor.Neighbor])
					{
						distances[neighbor.Neighbor] = newDist;
						pq.Enqueue(neighbor.Neighbor, newDist);
					}
				}
			}

			return distances;
		}

		public static int CalculateRouteCost(List<string> route, Dictionary<string, Dictionary<string, int>> distances)
		{
			int total = 0;
			for(int i = 0; i < route.Count - 1; i++)
			{
				total += distances[route[i]][route[i+1]];
			}
			return total;
		}

		public static List<string> TwoOpt(List<string> route, Dictionary<string, Dictionary<string, int>> distances)
		{
			var best = new List<string>(route);
			bool improved = true;

			while(improved)
			{
				improved = false;
				for(int i = 1; i < best.Count - 2; i++)
				{
					for(int k = i+1; k < best.Count - 1; k++)
					{
						var newRoute = TwoOptSwap(best, i, k);
						if(CalculateRouteCost(newRoute, distances) < CalculateRouteCost(best, distances))
						{
							best = newRoute;
							improved = true;
						}
					}
				}
			}

			return best;
		}

		private static List<string> TwoOptSwap(List<string> route, int i, int k)
		{
			var newRoute = new List<string>();
			newRoute.AddRange(route.Take(i));
			newRoute.AddRange(route.Skip(i).Take(k-i+1).Reverse());
			newRoute.AddRange(route.Skip(k+1));
			return newRoute;
		}

		public static List<string> TwoPointerOptimization(List<string> route, Dictionary<string, Dictionary<string, int>> distances)
		{
			var best = new List<string>(route);
			int n = best.Count;
		
			bool improved = true;
			while (improved)
			{
				improved = false;
		
				// Первый указатель — начало участка
				for (int i = 1; i < n - 2; i++)
				{
					// Второй указатель — конец участка
					int k = i + 1;
					while (k < n - 1)
					{
						// Вычисляем "стоимость рёбер" до и после потенциального обмена
						int a = distances[best[i - 1]][best[i]];
						int b = distances[best[k]][best[k + 1]];
						int c = distances[best[i - 1]][best[k]];
						int d = distances[best[i]][best[k + 1]];
		
						// Если перестановка улучшает маршрут — делаем swap
						if (c + d < a + b)
						{
							ReverseSegment(best, i, k);
							improved = true;
						}
		
						k++;
					}
				}
			}
		
			return best;
		}

		private static void ReverseSegment(List<string> route, int start, int end)
		{
			while (start < end)
			{
				var temp = route[start];
				route[start] = route[end];
				route[end] = temp;
				start++;
				end--;
			}
		}
	}
}
