using Banned.AniParser.Models.Enums;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Banned.AniParser.Native;

public static unsafe class Exports
{
    private static GCHandle MakeHandle(object   o)                 => GCHandle.Alloc(o, GCHandleType.Normal);
    private static T        GetTarget<T>(IntPtr h) where T : class => (T)GCHandle.FromIntPtr(h).Target!;

    [UnmanagedCallersOnly(EntryPoint = "Ani_Init", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Ani_Init(int globalization)
    {
        var g = globalization switch
        {
            1 => EnumChineseGlobalization.Simplified,
            2 => EnumChineseGlobalization.Traditional,
            _ => EnumChineseGlobalization.NotChange
        };
        var parser = new AniParser(o => o.Globalization = g);
        return GCHandle.ToIntPtr(MakeHandle(parser));
    }

    [UnmanagedCallersOnly(EntryPoint = "Ani_Destroy", CallConvs = [typeof(CallConvCdecl)])]
    public static void Ani_Destroy(IntPtr handle)
    {
        if (handle != IntPtr.Zero) GCHandle.FromIntPtr(handle).Free();
    }

    [UnmanagedCallersOnly(EntryPoint = "Ani_Parse", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Ani_Parse(IntPtr handle, byte* filenameUtf8)
    {
        try
        {
            var filename = Marshal.PtrToStringUTF8((nint)filenameUtf8)!;
            var parser   = GetTarget<AniParser>(handle);
            var result   = parser.Parse(filename);

            var json = JsonSerializer.Serialize(result, NativeJsonContext.Default.ParseResult);
            return Marshal.StringToCoTaskMemUTF8(json);
        }
        catch (Exception ex)
        {
            var err = JsonSerializer.Serialize(new ErrorDto { error = ex.Message }, NativeJsonContext.Default.ErrorDto);
            return Marshal.StringToCoTaskMemUTF8(err);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "Ani_ParseBatch", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Ani_ParseBatch(IntPtr handle, byte* jsonUtf8)
    {
        try
        {
            var span = new ReadOnlySpan<byte>(jsonUtf8, Len(jsonUtf8));

            var files = JsonSerializer.Deserialize(span, NativeJsonContext.Default.StringArray) ?? [];

            var parser = GetTarget<AniParser>(handle);
            var arr    = parser.ParseBatch(files).ToArray();

            var json = JsonSerializer.Serialize(arr, NativeJsonContext.Default.ParseResultArray);
            return Marshal.StringToCoTaskMemUTF8(json);

            static int Len(byte* p)
            {
                var i = 0;
                while (p[i] != 0) i++;
                return i;
            }
        }
        catch (Exception ex)
        {
            var err = JsonSerializer.Serialize(new ErrorDto { error = ex.Message }, NativeJsonContext.Default.ErrorDto);
            return Marshal.StringToCoTaskMemUTF8(err);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "Ani_GetParserList", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Ani_GetParserList(IntPtr handle)
    {
        try
        {
            var parser = GetTarget<AniParser>(handle);
            var list   = parser.GetParserList().ToArray();

            var json = JsonSerializer.Serialize(list, NativeJsonContext.Default.StringArray);
            return Marshal.StringToCoTaskMemUTF8(json);
        }
        catch (Exception ex)
        {
            var err = JsonSerializer.Serialize(new ErrorDto { error = ex.Message }, NativeJsonContext.Default.ErrorDto);
            return Marshal.StringToCoTaskMemUTF8(err);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "Ani_GetTranslationParserList", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Ani_GetTranslationParserList(IntPtr handle)
    {
        try
        {
            var parser = GetTarget<AniParser>(handle);
            var list   = parser.GetTranslationParserList().ToArray();

            var json = JsonSerializer.Serialize(list, NativeJsonContext.Default.StringArray);
            return Marshal.StringToCoTaskMemUTF8(json);
        }
        catch (Exception ex)
        {
            var err = JsonSerializer.Serialize(new ErrorDto { error = ex.Message }, NativeJsonContext.Default.ErrorDto);
            return Marshal.StringToCoTaskMemUTF8(err);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "Ani_GetTransferParserList", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Ani_GetTransferParserList(IntPtr handle)
    {
        try
        {
            var parser = GetTarget<AniParser>(handle);
            var list   = parser.GetTransferParserList().ToArray();

            var json = JsonSerializer.Serialize(list, NativeJsonContext.Default.StringArray);
            return Marshal.StringToCoTaskMemUTF8(json);
        }
        catch (Exception ex)
        {
            var err = JsonSerializer.Serialize(new ErrorDto { error = ex.Message }, NativeJsonContext.Default.ErrorDto);
            return Marshal.StringToCoTaskMemUTF8(err);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "Ani_GetCompressionParserList", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr Ani_GetCompressionParserList(IntPtr handle)
    {
        try
        {
            var parser = GetTarget<AniParser>(handle);
            var list   = parser.GetCompressionParserList().ToArray();

            var json = JsonSerializer.Serialize(list, NativeJsonContext.Default.StringArray);
            return Marshal.StringToCoTaskMemUTF8(json);
        }
        catch (Exception ex)
        {
            var err = JsonSerializer.Serialize(new ErrorDto { error = ex.Message }, NativeJsonContext.Default.ErrorDto);
            return Marshal.StringToCoTaskMemUTF8(err);
        }
    }


    [UnmanagedCallersOnly(EntryPoint = "Ani_Free", CallConvs = [typeof(CallConvCdecl)])]
    public static void Ani_Free(IntPtr ptr)
    {
        if (ptr != IntPtr.Zero) Marshal.FreeCoTaskMem(ptr);
    }
}
