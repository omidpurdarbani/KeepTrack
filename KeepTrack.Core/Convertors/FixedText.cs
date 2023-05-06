namespace KeepTrack.Core.Convertors
{
    public static class FixedText
    {
        public static string FixEmail(this string email)
        {
            return email.Trim().ToLower();
        }
    }
}
