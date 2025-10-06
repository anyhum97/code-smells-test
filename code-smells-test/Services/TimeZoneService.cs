using code_smells_test.Services.Interfaces;

namespace code_smells_test.Services
{
	public class TimeZoneService : ITimeZoneService
	{
		private readonly TimeZoneInfo _clientTimeZone;
		
		public TimeZoneService(IHttpContextAccessor httpContextAccessor)
		{
			var tzHeader = httpContextAccessor.HttpContext?.Request.Headers["X-TimeZone"].ToString();
			
			// По умолчанию UTC
			if(string.IsNullOrWhiteSpace(tzHeader))
			{
				_clientTimeZone = TimeZoneInfo.Utc;
				
				return;
			}
			
			try
			{
				if(tzHeader.StartsWith("+") || tzHeader.StartsWith("-"))
				{
					_clientTimeZone = TimeZoneInfo.CreateCustomTimeZone("CustomZone", TimeSpan.Parse(tzHeader), tzHeader, tzHeader);
				}
				else
				{
					_clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(tzHeader);
				}
			}
			catch
			{
				_clientTimeZone = TimeZoneInfo.Utc;
			}
		}
		
		public DateTime ConvertToUtc(DateTime localTime)
		{
			if(localTime.Kind == DateTimeKind.Utc)
			{
				return localTime;
			}
			
			return TimeZoneInfo.ConvertTimeToUtc(localTime, _clientTimeZone);
		}
		
		public DateTime ConvertFromUtc(DateTime utcTime)
		{
			return TimeZoneInfo.ConvertTimeFromUtc(utcTime, _clientTimeZone);
		}
	}
}
