﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLLRC
{
    class LLRC_Common
    {
        internal static int ALLOWED_BIG_SPACE_NUM = 4;

        internal static List<string> UNSUPPORT_ELBIT_TYPES = new List<string>()
        {
            "int",
            "char",
            "int16",
            "UINT8",
            "UINT16",
            "bool",
            "uint32_t",
        };

        internal static List<string> GLOBAL_FIELDS = new List<string>()
        {
            "ingroup",
            "Variable Name:",
            "Variable Type:",
            "Unit:",
            "Default value:",
            "Limits:",
            "Sign Convention:",
            "Description:",
            "Set by:",
            "Used by:",
        };

        internal static List<string> STRUCTURE_FIELDS = new List<string>()
        {
            "ingroup",
            "Structure Name:",
            "Description:",
            "Members Types:",
            "Members Names:",
            "Members Units:",
        };

        internal static List<string> FUNCTION_FIELDS = new List<string>()
        {
            @"\ingroup",
            @"\brief",
            @"\return",
            @"\FRSLinks_Common",
            @"\DerivedDesc",
            @"\Justification",
            @"\param",
        };

        internal static List<string> DEFINE_FIELDS = new List<string>()
        {
            "ingroup",
            "Define Name:",
            "Unit:",
            "Define Value:",
            "Description:",
        };

        internal static List<string> SOURCE_STRUCT_TITLES = new List<string>()
        {
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% INCLUDES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/* %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% DEFINES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% ENUMS %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% TYPES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% STRUCTURES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/* %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% GLOBAL VARIABLES %%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% LOCAL DECLARATIONS %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% FUNCTIONS IMPLEMENTATION %%%%%%%%%%%%%%%%%%%%%%%%%%% */",
        };

        internal static List<string> HEADER_STRUCT_TITLES = new List<string>()
        {
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% INCLUDES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/* %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% DEFINES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% STRUCTURES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% TYPES %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
            @"/*%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% PUBLIC DECLARATIONS %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */",
        };


    }
}