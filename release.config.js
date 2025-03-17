module.exports = {
  ci: false,
  branches: ["main", { name: "dev", prerelease: true }],
  plugins: [
    "@semantic-release/commit-analyzer",
    "@semantic-release/release-notes-generator",
    "@semantic-release/changelog",
  ],
};
