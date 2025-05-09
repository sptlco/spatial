// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets;

/// <summary>
/// Indicates the data type of a <see cref="Column"/>.
/// </summary>
public enum DataType
{
    BYTE = 1,
    WORD = 2,
    DWORD = 3,
    QWORD = 4,
    FLOAT = 5,
    STR = 9,
    STRAUTO = 10,
    INX = 11,
    INXBYTE = 12,
    INXWORD = 13,
    INXDWORD = 14,
    INXQWORD = 15,
    BYTE_BIT = 16,
    WORD_BIT = 17,
    DWORD_BIT = 18,
    QWORD_BIT = 19,
    BYTE_ARRAY = 20,
    WORD_ARRAY = 21,
    DWORD_ARRAY = 22,
    QWORD_ARRAY = 23,
    STR_ARRAY = 24,
    STRAUTO_ARRAY = 25,
    VARSTR = 26,
    INXSTR = 27,
    UNKNOWN = 28,
    KVP = 29
}