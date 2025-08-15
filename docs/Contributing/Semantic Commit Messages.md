[`🏠`](../README.md) > [`Contributing`](./README.md) > `Semantic Commit Messages`

# 🏷️ Semantic Commit Messages

Semantic commit messages help to understand the changes made in a commit. They follow a specific format that includes a type, scope, and subject.

## Pattern

```
<type>(<scope>): <subject>
```

### Examples:

```
feat(auth): add otp authenticator
fix(auth): correct user authentication flow
docs(readme): update installation instructions
style(ui): improve button styling
perf(api): optimize database queries
refactor(service): rename user service methods
test(unit): add tests for user service
build(deps): update dependency versions
ci(release): add new build step
```

## Segments

### Types

The type of commit message is used to categorize the changes made in a commit. The following types are commonly used:

- **feat**: A new feature for the user, not a new feature for build script
- **fix**: A bug fix for the user, not a fix to a build script
- **docs**: Changes to the documentation
- **style**: Formatting, missing semi colons, etc; no production code change
- **perf**: Production changes related to backward-compatible performance improvements
- **refactor**: Refactoring production code, e.g., renaming a variable
- **test**: Adding missing tests, refactoring tests; no production code change
- **build**: Updating build tasks, etc; no production code change
- **ci**: Changes to the continuous integration and deployment system - involving scripts, configurations, or tools

### Scopes

The scope of a commit message is used to specify the module or feature that the changes affect. It is optional but can be helpful in identifying the context of the changes.

#### Examples

- **auth**: Authentication module
- **customer**: Customer management features (e.g., registration, profile updates)
- **payment**: Payment processing features (e.g., handling transactions, refunds)
- **rental**: Rental process features (e.g., booking, returning cars)
- **dashboard**: Admin panel changes
- **reports**: Reporting systems changes

---

<details>
    <summary>References</summary>
    <ul>
        <li>https://www.conventionalcommits.org/</li>
        <li>https://seesparkbox.com/foundry/semantic_commit_messages</li>
        <li>http://karma-runner.github.io/1.0/dev/git-commit-msg.html</li>
        <li>https://nitayneeman.com/posts/understanding-semantic-commit-messages-using-git-and-angular/</li>
    </ul>
</details>
