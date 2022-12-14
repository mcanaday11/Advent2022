using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Advent2022
{
    public class Day13
    {

        public List<(Packet Left, Packet Right)> ThePairs;
        public List<Packet> Packets = new List<Packet>();
        public string result = "";

        public record PacketNumber(int Value) : Packet;
        public record PacketList(Packet[] Values) : Packet;
        public record Packet;


        public void GetPacketInput(string input)
        {
            ThePairs = GetPairs(input); //First solution
            Packets = GetAllInputNoBlankLines(input);
        }

        private List<(Packet Left, Packet Right)> GetPairs(string input) 
        {
            var pairs = input.Replace("\r\n", "\n")
                .Trim()
                .Split("\n\n")
                .Select(pair => pair.Split("\n"));

            return pairs
                .Select(segments => (
                    Left: FromJsonString(segments[0]),
                    Right: FromJsonString(segments[1])
                ))
                .ToList();
        }

        public string PacketResults(string input)
        {
            int answer = CalcValue2(Packets); //CalcValue1();
            result += String.Format("Answer: {0}\r\n", answer);
            return result;
        }

        private int CalcValue1()
        {
            PacketComparer cmp = new();

            return ThePairs
                .Select((pair, index) => (Order: cmp.Compare(pair.Left, pair.Right), Index: index + 1)) 
                .Where(data => data.Order == PacketComparer.Correct)
                .Sum(data => data.Index);
        }
        private int CalcValue2(List<Packet> packets)
        {
            PacketComparer cmp = new();
            var div2 = new PacketList(new Packet[] { new PacketNumber(2) }); //add a 2 divider
            var div6 = new PacketList(new Packet[] { new PacketNumber(6) }); //add a 6 divider

            packets.Add(div2);
            packets.Add(div6);
            packets.Sort(cmp);

            int i1 = packets.FindIndex(packet => packet == div2) + 1; 
            int i2 = packets.FindIndex(packet => packet == div6) + 1;
            return i1 * i2;
        }

        private List<Packet> GetAllInputNoBlankLines(string input)
        {
            var lines = input.Trim().Replace("\r\n\r\n", "\n").Replace("\r\n", "\n").Split('\n');
            return lines.Select(FromJsonString).ToList();
        }

        private static Packet FromJsonString(string json)
        {
            var element = (JsonElement)JsonSerializer.Deserialize<object>(json)!;
            return FromJsonElement(element);
        }

        private static Packet FromJsonElement(JsonElement element) =>
            element.ValueKind switch
            {
                JsonValueKind.Number => new PacketNumber(element.GetInt32()),
                JsonValueKind.Array => new PacketList(element.EnumerateArray().Select(FromJsonElement).ToArray())
            };

        public class PacketComparer : IComparer<Packet>
        {
            public const int Correct = -1;
            public const int Wrong = 1;
            public const int Same = 0;

            public int Compare(Packet? left, Packet? right)
            {
                return ComparePackets(left, right);
            }

            private static int ComparePackets(Packet left, Packet right)
            {
                if (left is PacketNumber leftNumber && right is PacketNumber rightNumber) //Both numbers, compare
                {
                    return leftNumber.Value.CompareTo(rightNumber.Value);
                }

                var leftList = ConvertToList(left);
                var rightList = ConvertToList(right);
                var min = Math.Min(leftList.Values.Length, rightList.Values.Length);

                if (leftList.Values.Length == rightList.Values.Length && min == 0) //Same numbers
                {
                    return Same;
                }

                for (var i = 0; i < min; ++i)
                {
                    var comparisonResult = ComparePackets(leftList.Values[i], rightList.Values[i]);
                    if (comparisonResult != 0)
                    {
                        return comparisonResult;
                    }
                }
                return leftList.Values.Length.CompareTo(rightList.Values.Length);
            }

            private static PacketList ConvertToList(Packet input) =>
                input switch
                {
                    PacketList pl => pl,
                    PacketNumber pn => new PacketList(new Packet[] { pn }),
                    _ => throw new ArgumentOutOfRangeException(nameof(input), input, "Unsupported input type")
                };
        }
    }
}
