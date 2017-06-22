using System.Collections.Generic;
using System.Text;

namespace AFPParser.StructuredFields
{
	public class MDD : StructuredField
	{
		private static string _abbr = "MDD";
		private static string _title = "Medium Descriptor";
		private static string _desc = "Specifies the size and orientation of the medium presentation space for all sheets that are generated by its medium map container.";
		private static List<Offset> _oSets = new List<Offset>()
        {
            new Offset(0, Lookups.DataTypes.CODE, "X Unit Base") { Mappings = CommonMappings.UnitBase },
            new Offset(1, Lookups.DataTypes.CODE, "Y Unit Base") { Mappings = CommonMappings.UnitBase },
            new Offset(2, Lookups.DataTypes.UBIN, "X Units per Base"),
            new Offset(4, Lookups.DataTypes.UBIN, "Y Units per Base"),
            new Offset(6, Lookups.DataTypes.UBIN, "X Extent"), // CUSTOM PARSED
            new Offset(9, Lookups.DataTypes.UBIN, "Y Extent"), // CUSTOM PARSED
            new Offset(12, Lookups.DataTypes.BITS, "Flags")
                { Mappings = new Dictionary<byte, string>() { { 0x01, "Do not pass orientation to printers|Pass orientation to printers" } } },
            new Offset(13, Lookups.DataTypes.TRIPS, "")
        };

		public override string Abbreviation => _abbr;
		public override string Title => _title;
		public override string Description => _desc;
		protected override bool IsRepeatingGroup => false;
		protected override int RepeatingGroupStart => 0;
		public override IReadOnlyList<Offset> Offsets => _oSets;

		public MDD(int length, string hex, byte flag, int sequence) : base (length, hex, flag, sequence) { }

        protected override string GetSingleOffsetDescription(Offset oSet, byte[] sectionedData)
        {
            if (oSet.StartingIndex < 6 || oSet.StartingIndex > 9)
                return base.GetSingleOffsetDescription(oSet, sectionedData);

            int size = (int)GetNumericValue(sectionedData, false);
            if (size > 0 && size <= short.MaxValue)
                return base.GetSingleOffsetDescription(oSet, sectionedData);

            StringBuilder sb = new StringBuilder($"{oSet.Description}: ");
            if (size == 0) sb.AppendLine("Not Specified");
            else sb.AppendLine("Presentation process default");
            return sb.ToString();
        }
    }
}