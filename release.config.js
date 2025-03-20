module.exports = {
  ci: true,
  branches: ["main", { name: "dev", prerelease: false }],
  plugins: [
    "@semantic-release/commit-analyzer",
    "@semantic-release/release-notes-generator",
    "@semantic-release/changelog",
    "@semantic-release/github",
    [
      "@semantic-release/exec",
      {
        successCmd:
          'echo "NEXT_SHORPY_VERSION=${nextRelease.version}" >> $GITHUB_ENV',
      },
    ],
  ],
};
