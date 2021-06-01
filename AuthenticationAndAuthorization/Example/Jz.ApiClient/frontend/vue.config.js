module.exports = {
  productionSourceMap: false,
  devServer: {
    open: true,
    host: "localhost",
    port: "8080",
    https: false,
    compress: true,
    proxy: {
      "/api": {
        target: "http://localhost:6500/",
        ws: true,
        changeOrigin: true,
        crossorigin: "anonymous",
        pathRewrite: {
          "^/api": ""
        }
      }
    }
  }
};
