import esbuild from "esbuild";

const watch = process.argv.includes("--watch");

const args = {
    entryPoints: ['./Assets/js/index.js', './Assets/css/index.css'],
    outdir: 'wwwroot',
    bundle: true,
    minify: true,
    platform: 'browser',
    sourcemap: true,
    target: 'esnext',
    loader: {
        '.woff2': 'dataurl',
    },
    logLevel: 'info'
};

(async () => {
    let ctx;
    if (watch) {
        ctx = await esbuild.context(args);
        await ctx.watch();
        console.log("\nwatching...\n");
    } else {
        ctx = await esbuild.build(args);
        console.log("build successful");
    }
})();
