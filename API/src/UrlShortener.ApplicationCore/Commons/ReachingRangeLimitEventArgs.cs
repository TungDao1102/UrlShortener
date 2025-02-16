namespace UrlShortener.ApplicationCore.Commons
{
    public class ReachingRangeLimitEventArgs : EventArgs
    {
        public long Token { get; set; }
        public long RangeLimit { get; set; }
    }
}
