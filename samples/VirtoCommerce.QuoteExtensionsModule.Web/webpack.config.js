const glob = require('glob');
const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');

const rootPath = path.resolve(__dirname, 'dist');

function getEntrypoints() {
    return [
        ...glob.sync('./Scripts/**/*.js', { nosort: true }),
        ...glob.sync('./Content/**/*.css', { nosort: true })
    ];
}

module.exports = [
    {
        entry: getEntrypoints(),
        output: {
            path: rootPath,
            filename: 'app.js'
        },
        module: {
            rules: [
                {
                    test: /\.css$/,
                    use: [MiniCssExtractPlugin.loader, 'css-loader'],
                }
            ]
        },
        devtool: false,
        plugins: [
            new webpack.SourceMapDevToolPlugin({
                namespace: 'VirtoCommerce.QuoteExtensionsModule'
            }),
            new CleanWebpackPlugin(rootPath, { verbose: true }),
            new MiniCssExtractPlugin({
                filename: 'style.css'
            })
        ]
    }
];
