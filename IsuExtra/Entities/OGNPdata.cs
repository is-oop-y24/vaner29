using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class OGNPdata
    {
        public OGNPdata(Guid studentId)
        {
            StudentId = studentId;
            OgnpIds = new List<Guid>();
        }

        public OGNPdata(Guid studentId, Guid ognpId)
        {
            StudentId = studentId;
            OgnpIds = new List<Guid>() { ognpId };
        }

        public OGNPdata(Guid studentId, List<Guid> ognpIds)
        {
            StudentId = studentId;
            OgnpIds = new List<Guid>(ognpIds);
        }

        public Guid StudentId { get; }
        public List<Guid> OgnpIds { get; }

        public OGNPdata SignStudentToOgnp(Guid ognpId)
        {
            if (OgnpIds.Count > 1)
                throw new IsuExtraException("Can't sign up to more than 2 OGNPs");
            if (OgnpIds.Count == 1 && OgnpIds[0] == ognpId)
                throw new IsuExtraException("Can't sign up to the same OGNP twice");
            OgnpIds.Add(ognpId);
            return new OGNPdata(StudentId, OgnpIds);
        }

        public OGNPdata RemoveStudentFromOgnp(Guid ognpId)
        {
            if (!OgnpIds.Any(id => id == ognpId)) throw new IsuExtraException("Student is not signed to this OGNP");
            OgnpIds.Remove(ognpId);
            return new OGNPdata(StudentId, OgnpIds);
        }
    }
}