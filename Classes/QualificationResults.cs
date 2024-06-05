using System;
using System.Collections.Generic;
using RetroSlices.Classes;

namespace RetroSlices;

public class QualificationResult
{
    public List<Customer> QualifiedCustomers { get; set; }
    public List<Customer> DeniedCustomers { get; set; }
    public int QualifiedCount { get; set; }
    public int DeniedCount { get; set; }
}

