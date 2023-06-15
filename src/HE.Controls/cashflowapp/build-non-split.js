const rewire = require('rewire');
const defaults = rewire('react-scripts/scripts/build.js');
const config = defaults.__get__('config');

config.optimization.splitChunks = {
    cacheGroups: {
        default: false,
    },
};

config.optimization.runtimeChunk = false;
config.output.filename = 'js/[name].js';
config.output.chunkFilename = "js/[name].chunk.js"

const miniCssExtractPlugin = config.plugins.find(element => element.constructor.name === "MiniCssExtractPlugin");
miniCssExtractPlugin.options.filename = "css/[name].css"
miniCssExtractPlugin.options.chunkFilename = "css/[name].css"