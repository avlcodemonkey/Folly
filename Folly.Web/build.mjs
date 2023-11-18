import esbuild from "esbuild";

const watch = process.argv.includes("--watch");

const args = {
    entryPoints: ['./Assets/ts/index.ts', './Assets/css/index.css'],
    outdir: 'Assets/dist',
    bundle: true,
    minify: true,
    platform: 'browser',
    sourcemap: true,
    target: 'esnext',
    loader: {
        '.woff2': 'dataurl',
    },
};

(async () => {
    let ctx;
    if (watch) {
        ctx = await esbuild.context(args);
        await ctx.watch();
        console.log("watching...");
    } else {
        ctx = await esbuild.build(args);
        console.log("build successful");
    }
})();
