using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;

namespace Fahrenheit.Tools.H2CS;

internal static class H2CSConfig {
    public static string DefaultNamespace = string.Empty;
    public static bool   EmitPrologue     = false;
    public static bool   EmitDeduplicated = true;
    public static string TypeAliasName    = string.Empty;
    public static string SrcPath          = string.Empty;
    public static string DestPath         = string.Empty;

    public static void CLIRead(FhH2CSArgs args) {
        DefaultNamespace = args.DefaultNamespace;
        EmitPrologue     = args.EmitPrologue;
        EmitDeduplicated = args.EmitDeduplicated;
        TypeAliasName    = args.TypeAliasName;
        SrcPath          = args.SrcPath;
        DestPath         = Directory.Exists(args.DestPath) ? args.DestPath : throw new Exception("E_INVALID_DEST_DIR");
    }
}

internal sealed record FhH2CSArgs(string DefaultNamespace,
                                  bool   EmitPrologue,
                                  bool   EmitDeduplicated,
                                  string TypeAliasName,
                                  string SrcPath,
                                  string DestPath);

internal class H2CSArgsBinder : BinderBase<FhH2CSArgs> {
    private readonly Option<string> _opt_def_ns;
    private readonly Option<bool>   _opt_emit_prolog;
    private readonly Option<bool>   _opt_emit_dedup;
    private readonly Option<string> _opt_type_name;
    private readonly Option<string> _opt_src_path;
    private readonly Option<string> _opt_dest_path;

    public H2CSArgsBinder(Option<string> optDefNs,
                          Option<bool>   optEmitProlog,
                          Option<bool>   optEmitDedup,
                          Option<string> optTypeName,
                          Option<string> optFilePath,
                          Option<string> optDestPath) {
        _opt_def_ns      = optDefNs;
        _opt_emit_prolog = optEmitProlog;
        _opt_emit_dedup  = optEmitDedup;
        _opt_type_name   = optTypeName;
        _opt_src_path    = optFilePath;
        _opt_dest_path   = optDestPath;
    }

    protected override FhH2CSArgs GetBoundValue(BindingContext binding_context) {
        string def_ns      = binding_context.ParseResult.GetValueForOption(_opt_def_ns) ?? throw new Exception("E_CLI_ARG_NULL");
        bool   emit_prolog = binding_context.ParseResult.GetValueForOption(_opt_emit_prolog);
        bool   emit_dedup  = binding_context.ParseResult.GetValueForOption(_opt_emit_dedup);
        string type_name   = binding_context.ParseResult.GetValueForOption(_opt_type_name) ?? throw new Exception("E_CLI_ARG_NULL");
        string src_path    = binding_context.ParseResult.GetValueForOption(_opt_src_path) ?? throw new Exception("E_CLI_ARG_NULL");
        string dest_path   = binding_context.ParseResult.GetValueForOption(_opt_dest_path) ?? throw new Exception("E_CLI_ARG_NULL");

        return new FhH2CSArgs(def_ns, emit_prolog, emit_dedup, type_name, src_path, dest_path);
    }
}
