using System;
using System.Collections.Generic;
using System.Text;
using Orleans.Core;

namespace Orleans.TestKit
{
    public sealed class TestGrainContextBuilder
    {
        public TestGrainContextBuilder()
        {
        }

        public TestGrainContext<TGrain> Build<TGrain>(long id) where TGrain : Grain, IGrainWithIntegerKey =>
            new TestGrainContext<TGrain>();
    }

    internal sealed class TestGrainIdentity : IEquatable<TestGrainIdentity>, IComparable<TestGrainIdentity>, IGrainIdentity
    {
        private readonly string extKeyString;

        public TestGrainIdentity(Guid primaryKey, long primaryKeyLong, string primaryKeyString, string extKeyString)
        {
            this.PrimaryKey = primaryKey;
            this.PrimaryKeyLong = primaryKeyLong;
            this.PrimaryKeyString = primaryKeyString;
            this.extKeyString = extKeyString;
        }

        public string IdentityString { get; }

        public bool IsClient =>
            throw new NotImplementedException();

        public Guid PrimaryKey { get; }

        public long PrimaryKeyLong { get; }

        public string PrimaryKeyString { get; }

        public int TypeCode =>
            throw new NotImplementedException();

        public int CompareTo(TestGrainIdentity other) => throw new NotImplementedException();

        public override bool Equals(object obj) =>
                    obj is TestGrainIdentity o && this.PrimaryKey == o.PrimaryKey && this.PrimaryKeyLong == o.PrimaryKeyLong &&
            this.PrimaryKeyString == o.PrimaryKeyString && this.extKeyString == o.extKeyString;

        public bool Equals(TestGrainIdentity other) => throw new NotImplementedException();

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)2166136261;
                hash = (hash * 16777619) ^ this.PrimaryKey.GetHashCode();
                hash = (hash * 16777619) ^ this.PrimaryKeyLong.GetHashCode();
                hash = (hash * 16777619) ^ (this.PrimaryKeyString == null ? 0 : this.PrimaryKeyString.GetHashCode());
                hash = (hash * 16777619) ^ (this.extKeyString == null ? 0 : this.extKeyString.GetHashCode());
                return hash;
            }
        }

        public Guid GetPrimaryKey(out string keyExt)
        {
            keyExt = this.extKeyString;
            return this.PrimaryKey;
        }

        public long GetPrimaryKeyLong(out string keyExt)
        {
            keyExt = this.extKeyString;
            return this.PrimaryKeyLong;
        }

        public uint GetUniformHashCode() =>
            unchecked((uint)this.GetHashCode());

        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("{0:x16}{1:x16}{2:x16}", N0, N1, TypeCodeData);
            if (!HasKeyExt)
                return s.ToString();

            s.Append("+");
            s.Append(KeyExt ?? "null");
            return s.ToString();
        }
    }
}
