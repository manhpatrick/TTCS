using System.Globalization;

namespace HotelManager.Infrastructure.Libraries
{
    // 2. Class phụ trợ để sắp xếp tham số theo Alphabet (Bắt buộc đối với VNPAY)
    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
