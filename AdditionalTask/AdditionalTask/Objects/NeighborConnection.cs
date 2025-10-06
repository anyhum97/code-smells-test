namespace AdditionalTask
{
    public readonly struct NeighborConnection
    {
		public NeighborConnection(string neighbor, int weight)
		{
			Neighbor = neighbor;
			Weight = weight;
		}

		public readonly string Neighbor;
		
		public readonly int Weight;
    }
}
